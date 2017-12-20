using System;
using System.Data;
using System.Data.SqlClient;

using System.IO;
using Telerik.Web.UI.Widgets;
using System.Web;
using System.Collections.Generic;

public class DBDataServer
{
	private readonly string connectionString;
	private readonly char pathSeparator;

	private DataTable _data;
	private DataTable Data
	{
		get
		{
			if (_data == null)
			{
				SqlConnection connection = this.GetConnection(this.connectionString);
				SqlDataAdapter adapter = new SqlDataAdapter("SELECT ItemID, [Name], ParentID, MimeType, IsDirectory, [Size], [Content] FROM Items ORDER BY ItemID, [Name]", connection);
				using (connection)
				{
					connection.Open();
					_data = new DataTable();
					adapter.Fill(_data);
				}
			}
			return _data;
		}
	}

	public DBDataServer(string connectionString) : this(connectionString, '/') { }
	public DBDataServer(string connectionString, char pathSeparator)
	{
		this.connectionString = connectionString;
		this.pathSeparator = pathSeparator;
	}

	private SqlConnection GetConnection(string connectionString)
	{
		return new SqlConnection(connectionString);
	}

	public void UpdateItem(string path, string newPath)
	{
		string oldName = this.GetName(path);
		string newName = this.GetName(newPath);


		int itemId = (int)GetItemIdFromPath(path);

		if (oldName == newName) //move
		{
			DataRow newParent = this.GetParentFromPath(newPath);
			this.UpdateItem(itemId, new string[] { "ParentID" }, new string[] { newParent["ItemID"].ToString() });
		}
		else //rename
		{
			this.UpdateItem(itemId, new string[] { "Name" }, new string[] { newName });
		}
	}

	public DataRow GetItem(string path)
	{
		return this.GetItemRowFromPath(path);
	}

	private void UpdateItem(int itemId, string[] fields, string[] values)
	{
		if (fields.Length != values.Length)
			return;

		string updateCommandStr = "UPDATE [Items] SET";
		for (int i = 0; i < fields.Length; i++)
		{
			updateCommandStr += String.Format(" [{0}]='{1}'", fields[i], values[i]);
			if (i < fields.Length - 1)
				updateCommandStr += ",";
		}
		updateCommandStr += String.Format(" WHERE [ItemID] = {0}", itemId);

		SqlConnection connection = this.GetConnection(this.connectionString);

		using (connection)
		{
			connection.Open();
			SqlCommand command = new SqlCommand(updateCommandStr, connection);
			command.ExecuteNonQuery();

			this._data = null; //force update
		}
	}

	private DataRow GetParentFromPath(string path)
	{
		string parentPath = path.Substring(0, TrimSeparator(path).LastIndexOf(pathSeparator));

		return this.GetItemRowFromPath(parentPath);
	}

	private int[] ConvertPathToIds(string path)
	{
		path = this.TrimSeparator(path);
		string[] names = path.Split('/');

		List<int> result = new List<int>(names.Length);

		int itemId = 0;
		for (int i = 0; i < names.Length; i++)
		{
			string name = names[i];
			DataRow[] rows = this.Data.Select(string.Format("Name='{0}' AND (ParentID={1} OR {1}=0)", name.Replace("'", "''"), itemId), "[Name]");
			if (rows.Length > 0)
			{
				result.Add((int)rows[0]["ItemID"]);
				itemId = (int)rows[0]["ItemID"];
			}
		}

		return names.Length == result.Count ? result.ToArray() : null;
	}

	private DataRow GetItemRowFromPath(string path)
	{
		int? itemId = GetItemIdFromPath(path);
		if (itemId == null) return null;

		DataRow[] result = this.Data.Select(String.Format("ItemID = {0}", itemId), "[Name]");
		return result.Length > 0 ? result[0] : null;
	}

	private int? GetItemIdFromPath(string path)
	{
		int[] ancestors = this.ConvertPathToIds(path);

		return ancestors != null && ancestors.Length > 0 ? (int?)ancestors[ancestors.Length - 1] : null;
	}

	public void DeleteItem(string path)
	{
		SqlConnection connection = this.GetConnection(this.connectionString);

		using (connection)
		{
			connection.Open();

			SqlCommand command = new SqlCommand(String.Format("DELETE FROM [Items] WHERE ItemID = {0}", GetItemIdFromPath(path)), connection);
			command.ExecuteNonQuery();

			this._data = null;
		}
	}

	private string AddItem(string name, int parentId, string mimeType, int isDirectory, long size, byte[] content)
	{
		try
		{
			SqlConnection connection = this.GetConnection(this.connectionString);

			SqlCommand command =
				new SqlCommand(
					"INSERT INTO Items ([Name], ParentId, MimeType, IsDirectory, [Size], Content) VALUES (@Name, @ParentId, @MimeType, @IsDirectory, @Size, @Content)", connection);
			command.Parameters.Add(new SqlParameter("@Name", name));
			command.Parameters.Add(new SqlParameter("@ParentId", parentId));
			command.Parameters.Add(new SqlParameter("@MimeType", mimeType));
			command.Parameters.Add(new SqlParameter("@IsDirectory", isDirectory));
			command.Parameters.Add(new SqlParameter("@Size", size));
			command.Parameters.Add(new SqlParameter("@Content", content));

			using (connection)
			{
				connection.Open();
				command.ExecuteNonQuery();
				this._data = null; //force update
			}

			return String.Empty;
		}
		catch (Exception e)
		{
			return e.Message;
		}
	}

	private string AddDirectory(string name, int parentId)
	{
		return this.AddItem(name, parentId, String.Empty, 1, 0, new byte[0]);
	}

	private string AddFile(string name, int parentId, string mimeType, byte[] content)
	{
		return this.AddItem(name, parentId, mimeType, 0, content.LongLength, content);
	}

	private void CopyItemInternal(string path, string newPath)
	{
		DataRow itemRow = this.GetItemRowFromPath(path);
		DataRow parent = this.GetParentFromPath(newPath);

		if (Convert.ToInt32(itemRow["IsDirectory"]) == 1)
		{
			this.AddDirectory(itemRow["Name"].ToString(), (int)parent["ItemID"]);

			DataRow[] children = this.Data.Select(String.Format("ParentId = {0}", (int)itemRow["ItemID"]), "[Name]");
			foreach (DataRow child in children)
			{
				this.CopyItemInternal(String.Format("{0}{1}{2}", this.TrimSeparator(path), this.pathSeparator, child["Name"].ToString()), String.Format("{0}{1}{2}", newPath, this.pathSeparator, child["Name"].ToString()));
			}
		}
		else
		{
			this.AddFile(itemRow["Name"].ToString(), (int)parent["ItemID"], itemRow["MimeType"].ToString(), (byte[])itemRow["Content"]);
		}
	}

	public void CopyItem(string path, string newPath)
	{
		this.CopyItemInternal(path, newPath);
		this._data = null;
	}

	public string StoreFile(string name, string location, string contentType, byte[] content)
	{
		DataRow parent = this.GetItemRowFromPath(location);
		if (parent == null) return "Invalid location path.";

		return AddFile(name, (int)parent["ItemID"], contentType, content);
	}

	public string CreateDirectory(string name, string location)
	{
		return this.AddDirectory(name, (int)this.GetItemIdFromPath(location));
	}

	public byte[] GetItemContent(string path)
	{
		DataRow item = this.GetItemRowFromPath(path);

		return item != null ? (byte[])item["Content"] : null;
	}

	public string GetPath(string path)
	{
		DataRow item = this.GetItemRowFromPath(path);
		if (item == null)
			item = this.GetParentFromPath(path);

		return Convert.ToInt32(item["IsDirectory"]) == 1 ? this.GetFullPath(item) : this.GetLoaction(item);
	}

	public string GetLocation(string path)
	{
		DataRow item = this.GetItemRowFromPath(path);

		return this.GetLoaction(item);
	}

	private string GetLoaction(DataRow item)
	{
		if (String.IsNullOrEmpty(item["ParentID"].ToString()))
		{
			return String.Empty;
		}

		DataRow parentFolder = this.Data.Select(String.Format("ItemID = {0}", item["ParentID"].ToString()), "[Name]")[0];
		return this.GetFullPath(parentFolder);
	}

	public FileItem GetFileItem(string path, string handlerPath)
	{
		DataRow item = this.GetItemRowFromPath(path);

		return this.CreateFileItem(item, handlerPath);
	}

	public DirectoryItem GetDirectoryItem(string path, bool includeSubfolders)
	{
		DataRow item = this.GetItemRowFromPath(path);

		return (item != null && Convert.ToInt32(item["IsDirectory"]) == 1) ? this.CreateDirectoryItem(item, includeSubfolders) : null;
	}

	private DirectoryItem CreateDirectoryItem(DataRow item, bool includeSubfolders)
	{
		DirectoryItem directory = new DirectoryItem(item["Name"].ToString(),
													this.GetLoaction(item),
													this.GetFullPath(item),
													String.Empty,
													PathPermissions.Read, //correct permissions should be applied from the content provider
													null,
													null
													);

		if (includeSubfolders)
		{
			DataRow[] subDirItems = GetChildDirectories(item);
			List<DirectoryItem> subDirs = new List<DirectoryItem>();

			foreach (DataRow subDir in subDirItems)
			{
				subDirs.Add(CreateDirectoryItem(subDir, false));
			}

			directory.Directories = subDirs.ToArray();
		}

		return directory;
	}

	private DataRow[] GetChildDirectories(DataRow item)
	{
		return this.Data.Select(String.Format("ParentID = {0} AND IsDirectory = 1", item["ItemID"].ToString()), "[Name]");
	}

	public FileItem[] GetChildFiles(string folderPath, string[] searchPatterns, string handlerPath)
	{
		DataRow parentFolder = this.GetItemRowFromPath(folderPath);

		DataRow[] fileRows = this.Data.Select(String.Format("ParentID = {0} AND IsDirectory = 0{1}", parentFolder["ItemID"].ToString(), this.GetSearchPatternsFilter(searchPatterns)), "[Name]");

		List<FileItem> result = new List<FileItem>(fileRows.Length);
		foreach (DataRow fileRow in fileRows)
		{
			result.Add(this.CreateFileItem(fileRow, handlerPath));
		}

		return result.ToArray();
	}

	private string GetSearchPatternsFilter(string[] searchPatterns)
	{
		if (Array.IndexOf(searchPatterns, "*.*") > -1)
			return String.Empty;


		string searchPatterntsFilterExpression = " AND (Name LIKE '%";
		for (int i = 0; i < searchPatterns.Length; i++)
		{
			searchPatterntsFilterExpression += searchPatterns[i].Substring(searchPatterns[i].LastIndexOf('.'));
			if (i < searchPatterns.Length - 1)
				searchPatterntsFilterExpression += "' OR Name LIKE '%";
			else
				searchPatterntsFilterExpression += "')";
		}

		return searchPatterntsFilterExpression;
	}

	private FileItem CreateFileItem(DataRow item, string handlerPath)
	{
		string itemPath = this.GetFullPath(item);
		return new FileItem(item["Name"].ToString(),
							Path.GetExtension(itemPath),
							Convert.ToInt64(item["Size"]),
							itemPath,
							GetItemUrl(itemPath, handlerPath),
							String.Empty,
							PathPermissions.Read //correct permissions should be applied from the content provider
							);
	}

	private string GetFullPath(DataRow item)
	{
		string path = item["Name"].ToString();
		if (Convert.ToInt32(item["IsDirectory"]) == 1) path += this.pathSeparator;

		do
		{
			DataRow[] parentSearch = !String.IsNullOrEmpty(item["ParentID"].ToString()) ? this.Data.Select(String.Format("ItemID = {0}", item["ParentID"].ToString()), "[Name]") : new DataRow[0];

			if (parentSearch.Length > 0)
			{
				item = parentSearch[0];
				path = String.Format("{0}{1}{2}", item["Name"].ToString(), this.pathSeparator, path);
			}
		} while (!String.IsNullOrEmpty(item["ParentID"].ToString()));

		return path;
	}

	private string GetItemUrl(string itemPath, string handlerPath)
	{
		string escapedPath = HttpUtility.UrlEncode(itemPath);
		return string.Format("{0}?path={1}", handlerPath, escapedPath);
	}

	public bool ItemExists(string path)
	{
		DataRow item = this.GetItemRowFromPath(path);
		return !Object.Equals(item, null);
	}

	private string TrimSeparator(string path)
	{
		return path.Trim(this.pathSeparator);
	}

	private string GetName(string path)
	{
		string tmpPath = this.TrimSeparator(path);
		return tmpPath.Substring(tmpPath.LastIndexOf(pathSeparator) + 1);
	}

}
