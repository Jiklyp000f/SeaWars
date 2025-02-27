using System;
using System.Collections.Generic;

namespace SeaWars
{
    public enum CellType //создание произвольных переменных для полей
    {
        CloseNull, // закрыто - ничего
        CloseLife, // закрыто - живой(корабль) ДЛЯ ПРОТИВНИКА
        OpenLoss, // открыто - промах
        OpenHit, // открыто - попал(по кораблю)
        OpenNull, //открыто - ничего
        OpenLife // открыто - живой(корабль) ДЛЯ ИГРОКА
    }
    public class playseabattle //логика игры
    {
        Random rnd = new Random();
        public CellType[,] user_field = new CellType[ 10, 10 ]; // поле игрока
        public CellType[,] enemy_field = new CellType[ 10, 10 ]; // поле противника
        public int cellH = 0;
        public int cellHEnemy = 0;
        public int cellW = 0;
        public int cellWEnemy = 0;
        public static int Turn = 0;

        public void Init( int w, int h, int wEn, int hEn ) // метод инициализации поля
        {
            cellH = ( h - 9 ) / 10;
            cellW = ( w - 9 ) / 10;
            cellHEnemy = ( hEn - 9 ) / 10;
            cellWEnemy = ( wEn - 9 ) / 10;



            for ( int i = 0; i < 10; i++ )
            {
                for ( int j = 0; j < 10; j++ )
                {
                    enemy_field[ i, j ] = CellType.CloseNull; // начальная инициализация поля противника(закрыто - ничего)
                }
            }

            for ( int i = 0; i < 10; i++ )
            {
                for ( int j = 0; j < 10; j++ )
                {
                    user_field[ i, j ] = CellType.OpenNull;// начальная инициализация поля игрока(открыто - ничего)
                }
            }
        }



        public List<Ship> UserStartToWar() // метод для начала игры(расстановки) ИГРОКА
        {
            List<Ship> ships = new List<Ship>();

            ships.Add( new Ship( CellType.OpenLife, 1, true ) );
            ships.Add( new Ship( CellType.OpenLife, 1, true ) );
            ships.Add( new Ship( CellType.OpenLife, 1, true ) );
            ships.Add( new Ship( CellType.OpenLife, 1, true ) );
            ships.Add( new Ship( CellType.OpenLife, 2, true ) );
            ships.Add( new Ship( CellType.OpenLife, 2, true ) );
            ships.Add( new Ship( CellType.OpenLife, 2, true ) );
            ships.Add( new Ship( CellType.OpenLife, 3, true ) );
            ships.Add( new Ship( CellType.OpenLife, 3, true ) );
            ships.Add( new Ship( CellType.OpenLife, 4, true ) );

            return ships;
        }

        public List<Ship> EnemyStartToWar() // метод для начала игры(расстановки) ПРОТИВНИКА
        {
            List<Ship> ships = new List<Ship>();

            ships.Add( new Ship( CellType.CloseLife, 1, true ) );
            ships.Add( new Ship( CellType.CloseLife, 1, true ) );
            ships.Add( new Ship( CellType.CloseLife, 1, true ) );
            ships.Add( new Ship( CellType.CloseLife, 1, true ) );
            ships.Add( new Ship( CellType.CloseLife, 2, true ) );
            ships.Add( new Ship( CellType.CloseLife, 2, true ) );
            ships.Add( new Ship( CellType.CloseLife, 2, true ) );
            ships.Add( new Ship( CellType.CloseLife, 3, true ) );
            ships.Add( new Ship( CellType.CloseLife, 3, true ) );
            ships.Add( new Ship( CellType.CloseLife, 4, true ) );

            return ships;
        }

        public void TernTime( bool User, bool Enemy ) //счётчик ходов
        {
            if ( User == true && Enemy == true )
            {
                Turn++;
                User = false;
                Enemy = false;
            }

        }


        public void autoPutShips() // авторасстановка
        {
            enemy_field[ 0, 0 ] = CellType.CloseLife;
        }


        public void delShip( int xM, int yM ) // удалить точку на карте
        {
            int x = xM / cellW;
            int y = yM / cellH;

            user_field[ x, y ] = CellType.OpenNull;
        }

        public void putShip( int xM, int yM ) //поставить точку на карте
        {
            int x = xM / cellW;
            int y = yM / cellH;

            user_field[ x, y ] = CellType.OpenLife;
        }

        public bool TernUser( int xM, int yM ) // логика хода игрока
        {
            int x = xM / cellWEnemy;
            int y = yM / cellHEnemy; // логика отрисовки врага

            if ( enemy_field[ x, y ] == CellType.CloseLife )
            {
                enemy_field[ x, y ] = CellType.OpenHit;
            }



            return true;
        }

        public bool TernEnemy( int xM, int yM ) // логика хода противника
        {
            int x = xM / cellW;
            int y = yM / cellH;



            return true;
        }


        public playseabattle() { } //конструктор по умолчанию для создания игры
    }
}
