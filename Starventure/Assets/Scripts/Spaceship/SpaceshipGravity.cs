using Starventure.Planets;
using UnityEngine;

namespace Starventure.Spaceship {
    public class SpaceshipGravity : PlanetGravity {
        public override Vector3 CalculateGravityDirection(Vector3 objectPosition) {
            return -core.up;
        }
    }
}
