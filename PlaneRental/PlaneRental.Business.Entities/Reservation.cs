using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace PlaneRental.Business.Entities
{
    [DataContract]
    public class Reservation : EntityBase, IIdentifiableEntity//, IAccountOwnedEntity
    {
        [DataMember]
        public int ReservationId { get; set; }

        [DataMember]
        public string AccountId { get; set; }

        [DataMember]
        public int PlaneId { get; set; }

        [DataMember]
        public DateTime RentalDate { get; set; }

        [DataMember]
        public DateTime ReturnDate { get; set; }

        #region IIdentifiableEntity members

        public int EntityId
        {
            get { return ReservationId; }
            set { ReservationId = value; }
        }

        #endregion

        #region IAccountOwnedEntity members

        /*int IAccountOwnedEntity.OwnerAccountId
        {
            get { return AccountId; }
        }*/

        #endregion
    }
}