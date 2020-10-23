using System.Collections.Generic;

namespace Dennis.Domain.Abstractions
{
    public abstract class Entity : IEntity
    {
        public abstract object[] GetKeys();

        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Keys= {string.Join(",", this.GetKeys())}";
        }

        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => this._domainEvents?.AsReadOnly();

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            this._domainEvents = this._domainEvents ?? new List<IDomainEvent>();
            this._domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            this._domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            this._domainEvents?.Clear();
        }
    }

    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        public TKey Id { get; protected set; }
        public override object[] GetKeys()
        {
            return new object[] { this.Id };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            Entity<TKey> item = (Entity<TKey>)obj;
            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.Id.Equals(this.Id);
            }
        }

        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(this.Id, default);
        }

        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Id = {this.Id}";
        }

        public static bool operator == (Entity<TKey> left, Entity<TKey> right)
        {
            //if(object.Equals(left, null))
            //{
            //    return object.Equals(right, null) ? true : false;
            //}
            //else
            //{
            //    return left.Equals(right);
            //}
            if(ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }

            return ReferenceEquals(left, null) || left.Equals(right);
        }

        public static bool operator != (Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }
    }
}