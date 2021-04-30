﻿using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ComplaintService<T> : CrudService<T>, IComplaintService<T> where T : Complaint
    {
        public ComplaintService(IRepository<T> repository) :
            base(repository)
        {
        }

        public IEnumerable<T> ReadBy(Guid writerId)
        {
            return Read().Where(complaint => complaint.WriterId == writerId).ToList();
        }
    }
}