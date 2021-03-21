﻿namespace Farmacio_Models.Domain
{
    public class Account : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool IsVerified { get; set; }
        public bool ShouldChangePassword { get; set; }
        public virtual User User { get; set; }
    }

    public enum Role
    {
        Patient,
        Supplier,
        Pharmacist,
        Dermatologist,
        PharmacyAdmin,
        SystemAdmin
    }
}