﻿using CRM.Domain.Events;
using System;
using System.Collections.Generic;
// TODO: Was first .ToImmutableArray();
//using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace CRM.Domain.Common
{
    public abstract class BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();

        protected BaseAggregateRoot() { }
        
        protected BaseAggregateRoot(TKey id) : base(id)
        {
        }

        // TODO: Was first _events.ToImmutableArray();
        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToList();

        public long Version { get; private set; }

        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AddEvent(IDomainEvent<TKey> @event)
        {
            _events.Enqueue(@event);
           
            this.Apply(@event);

            this.Version++;
        }

        protected abstract void Apply(IDomainEvent<TKey> @event);

        #region Factory

        private static readonly Lazy<ConstructorInfo> LazyCtor;

        static BaseAggregateRoot()
        {
            LazyCtor = new Lazy<ConstructorInfo>(() =>
            {
                var aggregateType = typeof(TA);
                var ctor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[0], new ParameterModifier[0]);
                return ctor;
            });
        }

        public static TA Create(IEnumerable<IDomainEvent<TKey>> events)
        {
            if(null == events || !events.Any())
                throw new ArgumentNullException(nameof(events));

            var cTor = LazyCtor.Value;
            var result = (TA)cTor.Invoke(new object[0]);

            var baseAggregate =  result as BaseAggregateRoot<TA, TKey>;
            if (baseAggregate != null) 
                foreach (var @event in events)
                    baseAggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion Factory
    }
}