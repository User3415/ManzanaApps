using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Manzana.Domain.Entities
{
    /// <summary>
    /// Cheque model
    /// </summary>
    [DataContract]
    public class Cheque : IValidatableObject
    {
        /// <summary>
        /// Identificator cheque
        /// </summary>
        [DataMember]
        public int ChequeId { get; set; }

        /// <summary>
        /// Cheque number
        /// </summary>
        [DataMember]
        [Required]
        public int ChequeNumber { get; set; }

        /// <summary>
        /// Cheque sum
        /// </summary>
        [DataMember]
        [Required]
        public decimal Sum { get; set; }

        /// <summary>
        /// Cheque discount
        /// </summary>
        [DataMember]
        public decimal Discount { get; set; }

        /// <summary>
        /// Articles
        /// </summary>
        [DataMember]
        public string[]  Articles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Sum <= 0L)
            {
                yield return new ValidationResult("Сумма должна быть больше 0.", new[] { "Sum" });
            }
            if(ChequeNumber < 1)
            {
                yield return new ValidationResult("Номер чека не может быть 0.", new[] { "ChequeNumber" });
            }
        }
    }
}
