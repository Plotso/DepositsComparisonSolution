﻿namespace DepositsComparison.Data.Models.Asbtract
{
    using System;

    public class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}