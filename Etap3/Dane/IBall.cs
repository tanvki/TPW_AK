using System.Numerics;

namespace Data
{

    public interface IBall : IDisposable
    {
        int  Id { get; }
        int Diameter { get; }


        Vector2 Position { get; }
        Vector2 Velocity { get; set; }

        int Mass { get; }

        event EventHandler? BallChanged;

        
    
    }
}
