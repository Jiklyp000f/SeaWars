namespace SeaWars
{
    public class Ship // класс отвечающий за объект КОРАБЛЬ
    {
        public CellType Life { get; set; } // стартовая точка расстановки
        public int Length { get; set; } // длина корабля
        public bool IsHorizontal { get; set; } // проверка на горизонтальность/вертикальность

        public Ship( CellType life, int length, bool isHorizontal )
        {
            Life = life;
            Length = length;
            IsHorizontal = isHorizontal;
        }
    }
}
