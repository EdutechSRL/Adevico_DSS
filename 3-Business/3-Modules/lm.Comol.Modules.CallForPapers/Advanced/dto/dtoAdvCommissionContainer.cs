using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.Federation;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Advanced.dto
{
    /// <summary>
    /// dto Commissione
    /// </summary>
    public class dtoAdvCommissionContainer
    {
        /// <summary>
        /// Id commissione
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// Nome Commissione
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descrizione commissione
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Stato commissione
        /// </summary>
        public CommissionStatus Status { get; set; }
        /// <summary>
        /// Se commissione Master (principale)
        /// </summary>
        public bool IsMaster { get; set; }
        /// <summary>
        /// Premessi commissione
        /// </summary>
        public CommissionPermission Permission { get; set; }
        /// <summary>
        /// Permessi generici (step)
        /// </summary>
        public GenericStepPermission GenericPermission { get; set; }

        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoAdvCommissionContainer()
        {
            Id = 0;
            Name = "";
            Description = "";
            Status = CommissionStatus.Draft;
            IsMaster = false;
            Permission = CommissionPermission.None;
            GenericPermission = GenericStepPermission.none;


        }

        //public dtoAdvCommissionContainer(Domain.AdvCommission commission)
        //{
        //    Id = commission.Id;
        //    Name = commission.Name;
        //    Description = commission.Description;
        //    Status = commission.Status;
        //    IsMaster = commission.IsMaster;
        //    Permission = CommissionPermission.None;
        //}

        /// <summary>
        /// Costruttore su commissione
        /// </summary>
        /// <param name="commission">Commissione di riferimento</param>
        /// <param name="currentUserId">Id utente corrente, per calcolo permessi</param>
        public dtoAdvCommissionContainer(Domain.AdvCommission commission, int currentUserId)
        {
            Id = commission.Id;
            Name = commission.Name;
            Description = commission.Description;
            Status = commission.Status;
            IsMaster = commission.IsMaster;

            if (currentUserId > 0)
            {
                Permission = CommissionPermission.None;
                
                if (commission.President.Id == currentUserId)
                {
                    Permission |= CommissionPermission.View
                        | CommissionPermission.Edit
                        | CommissionPermission.UploadVerbale;

                    GenericPermission |= GenericStepPermission.President;

                    if(commission.IsMaster)
                        GenericPermission |= GenericStepPermission.MainPresident;
                }

                if (commission.Secretary.Id == currentUserId)
                {
                    Permission |= CommissionPermission.View
                        | CommissionPermission.Edit
                        | CommissionPermission.UploadVerbale
                        | CommissionPermission.RequestIntegration;

                    GenericPermission |= GenericStepPermission.Secretary;

                    if (commission.IsMaster)
                        GenericPermission |= GenericStepPermission.MainSecretary;
                }


                if (commission.Members.Any(m => m.Member != null && m.Member.Id == currentUserId))
                {
                    if (commission.Status != CommissionStatus.Draft)
                        Permission |= CommissionPermission.View;

                    Permission |= CommissionPermission.Evaluate;

                    GenericPermission |= GenericStepPermission.Member;
                }
                
            }
        }

        /// <summary>
        /// Verifica permessi
        /// </summary>
        /// <param name="required">Permesso da valutare</param>
        /// <returns>True se ha il permesso da valutare</returns>
        public bool HasCommissionPermission(CommissionPermission required)
        {
            return ((required & Permission) == required);
        }


    }
}
