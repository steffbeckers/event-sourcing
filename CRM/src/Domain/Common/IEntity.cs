﻿namespace CRM.Domain.Common
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}