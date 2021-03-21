using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Patient : User
    {
        public int Points { get; set; }
        public int NegativePoints { get; set; }
        public virtual LoyaltyProgram LoyaltyProgram { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<Pharmacy> FollowedPharmacies { get; set; }
        public virtual List<Ingredient> Allergies { get; set; }
    }
}
