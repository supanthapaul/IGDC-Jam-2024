namespace Health_System
{
    public interface IHealth
    {
        public int currentHealth { get; }
        public int totalHealth { get; }

        public bool isAlive { get; }
        
        public void TakeDamage(int damage);
    }
}