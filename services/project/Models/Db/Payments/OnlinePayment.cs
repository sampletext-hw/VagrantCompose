using System;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.DbOrder;

namespace Models.Db.Payments;

public class OnlinePayment : IdEntity
{
    [ForeignKey(nameof(Order))]
    public long OrderId { get; set; }

    public virtual Order Order { get; set; }

    [ForeignKey(nameof(Issuer))]
    public long IssuerId { get; set; }

    public virtual WorkerAccount Issuer { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public PaymentStatus PaymentStatus { get; set; }
    
    public ConfirmationSource ConfirmationSource { get; set; }

    public string ExternalId { get; set; }
}