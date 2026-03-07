using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Position
{
    public class Position
    {
        public Position(Id id, Name name, Description descreption, EntityLifeTime lifeTime)
        {
            Id = id;
            Name = name;
            Descreption = descreption;
            LifeTime = lifeTime;
        }
        public Id Id { get; }
        public Name Name { get; }
        public Description Descreption { get; }
        public EntityLifeTime LifeTime { get; }
    }
}       