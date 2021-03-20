using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.Domain
{
    public class Patient : User
    {
        public int LoyalityPoints { get; set; }
        public int NegativePoints { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Pharmacy> FollowedPharmacies { get; set; }
        public List<Ingredient> Allergies { get; set; }
    }
}
