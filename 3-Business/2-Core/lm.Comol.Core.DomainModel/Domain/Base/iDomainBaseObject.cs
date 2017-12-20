
namespace lm.Comol.Core.DomainModel
{
	public interface iDomainBaseObject<T>
	{
        T Id { get; set; }
        BaseStatusDeleted Deleted { get; set; }
        byte[] TimeStamp { get; set; }
	}
}