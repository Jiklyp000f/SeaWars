namespace SeaWars
{
    public class Ship
    {
        public CellType Life { get; set; } // Тип клетки (OpenLife для игрока, CloseLife для противника)
        public int Length { get; set; } // Длина корабля
        public bool IsHorizontal { get; set; } // Ориентация корабля
        public int Hits { get; set; } // Количество попаданий по кораблю

        public Ship( CellType life, int length, bool isHorizontal )
        {
            Life = life;
            Length = length;
            IsHorizontal = isHorizontal;
            Hits = 0; // Изначально корабль не поврежден
        }

        public bool IsDestroyed() // Проверка, уничтожен ли корабль
        {
            return Hits >= Length;
        }
    }
}