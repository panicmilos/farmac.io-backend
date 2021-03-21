﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.Contracts.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        bool Active { get; set; }
    }
}