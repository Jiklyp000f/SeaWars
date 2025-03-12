using System;
using System.Collections.Generic;

namespace SeaWars
{
    public enum CellType
    {
        CloseNull, // Закрыто - ничего
        CloseLife, // Закрыто - живой (корабль) для противника
        OpenLoss, // Открыто - промах
        OpenHit, // Открыто - попал (по кораблю)
        OpenNull, // Открыто - ничего
        OpenLife // Открыто - живой (корабль) для игрока
    }

    public class playseabattle
    {
        private Random rnd = new Random();
        public CellType[,] user_field = new CellType[ 10, 10 ]; // Поле игрока
        public CellType[,] enemy_field = new CellType[ 10, 10 ]; // Поле противника
        public int cellH, cellW, cellHEnemy, cellWEnemy; // Размеры ячеек
        public static int Turn = 0; // Счетчик ходов

        public void Init( int w, int h, int wEn, int hEn ) // Инициализация размеров полей
        {
            cellH = ( h - 9 ) / 10;
            cellW = ( w - 9 ) / 10;
            cellHEnemy = ( hEn - 9 ) / 10;
            cellWEnemy = ( wEn - 9 ) / 10;

            for ( int i = 0; i < 10; i++ )
            {
                for ( int j = 0; j < 10; j++ )
                {
                    user_field[ i, j ] = CellType.OpenNull; // Инициализация поля игрока
                    enemy_field[ i, j ] = CellType.CloseNull; // Инициализация поля противника
                }
            }
        }

        public List<Ship> UserStartToWar() // Создание списка кораблей для игрока
        {
            List<Ship> ships = new List<Ship>
            {
                new Ship(CellType.OpenLife, 1, true),
                new Ship(CellType.OpenLife, 1, true),
                new Ship(CellType.OpenLife, 1, true),
                new Ship(CellType.OpenLife, 1, true),
                new Ship(CellType.OpenLife, 2, true),
                new Ship(CellType.OpenLife, 2, true),
                new Ship(CellType.OpenLife, 2, true),
                new Ship(CellType.OpenLife, 3, true),
                new Ship(CellType.OpenLife, 3, true),
                new Ship(CellType.OpenLife, 4, true)
            };

            return ships;
        }

        public List<Ship> EnemyStartToWar() // Создание списка кораблей для противника
        {
            List<Ship> ships = new List<Ship>
            {
                new Ship(CellType.CloseLife, 1, true),
                new Ship(CellType.CloseLife, 1, true),
                new Ship(CellType.CloseLife, 1, true),
                new Ship(CellType.CloseLife, 1, true),
                new Ship(CellType.CloseLife, 2, true),
                new Ship(CellType.CloseLife, 2, true),
                new Ship(CellType.CloseLife, 2, true),
                new Ship(CellType.CloseLife, 3, true),
                new Ship(CellType.CloseLife, 3, true),
                new Ship(CellType.CloseLife, 4, true)
            };

            return ships;
        }

        public void AutoPlaceShips( CellType[,] field, List<Ship> ships ) // Автоматическая расстановка кораблей
        {
            foreach ( var ship in ships )
            {
                int startX, startY;

                do
                {
                    startX = rnd.Next( 0, 10 );
                    startY = rnd.Next( 0, 10 );
                } while ( !CanPlaceShip( startX, startY, ship.Length, ship.IsHorizontal, field ) );

                if ( ship.IsHorizontal )
                {
                    for ( int i = 0; i < ship.Length; i++ )
                    {
                        field[ startX + i, startY ] = ship.Life;
                    }
                }
                else
                {
                    for ( int i = 0; i < ship.Length; i++ )
                    {
                        field[ startX, startY + i ] = ship.Life;
                    }
                }
            }
        }

        private bool CanPlaceShip( int startX, int startY, int length, bool isHorizontal, CellType[,] field ) // Проверка возможности размещения корабля
        {
            if ( isHorizontal )
            {
                if ( startX + length > 10 )
                    return false;
                for ( int i = 0; i < length; i++ )
                {
                    if ( field[ startX + i, startY ] != CellType.CloseNull && field[ startX + i, startY ] != CellType.OpenNull )
                    {
                        return false;
                    }
                }
            }
            else
            {
                if ( startY + length > 10 )
                    return false;
                for ( int i = 0; i < length; i++ )
                {
                    if ( field[ startX, startY + i ] != CellType.CloseNull && field[ startX, startY + i ] != CellType.OpenNull )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool TernUser( int xM, int yM ) // Ход игрока
        {
            int x = xM / cellWEnemy;
            int y = yM / cellHEnemy;

            if ( enemy_field[ x, y ] == CellType.CloseLife )
            {
                enemy_field[ x, y ] = CellType.OpenHit;
                return true;
            }
            else if ( enemy_field[ x, y ] == CellType.CloseNull )
            {
                enemy_field[ x, y ] = CellType.OpenLoss;
                return true;
            }

            return false;
        }

        public bool TernEnemy( int xM, int yM ) // Ход противника
        {
            int x = xM / cellW;
            int y = yM / cellH;

            if ( user_field[ x, y ] == CellType.OpenLife )
            {
                user_field[ x, y ] = CellType.OpenHit;
                return true;
            }
            else if ( user_field[ x, y ] == CellType.OpenNull )
            {
                user_field[ x, y ] = CellType.OpenLoss;
                return true;
            }

            return false;
        }

        public bool IsPlayerWin() // Проверка победы игрока
        {
            foreach ( var cell in enemy_field )
            {
                if ( cell == CellType.CloseLife )
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsEnemyWin() // Проверка победы противника
        {
            foreach ( var cell in user_field )
            {
                if ( cell == CellType.OpenLife )
                {
                    return false;
                }
            }
            return true;
        }
    }
}