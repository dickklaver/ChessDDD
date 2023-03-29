using CSharpFunctionalExtensions;

namespace Chess.Core
{
    public class Entity : Entity<Guid>
    {
        public Entity(): base(Guid.NewGuid())
        {            
        }
       
    }




}