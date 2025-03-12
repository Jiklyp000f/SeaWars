using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars
{
    public partial class Form1 : Form
    {
        private playseabattle current_play = null;
        private List<Ship> ships; // Список доступных кораблей

        public Form1()
        {
            InitializeComponent();
        }

        private void новаяToolStripMenuItem_Click( object sender, EventArgs e ) // Новая игра
        {
            current_play = new playseabattle();
            current_play.Init( pictureBox1.Width, pictureBox1.Height, pictureBox2.Width, pictureBox2.Height );

            // Создаем список кораблей для игрока
            ships = current_play.UserStartToWar();

            // Автоматически расставляем корабли противника
            var enemyShips = current_play.EnemyStartToWar();
            current_play.AutoPlaceShips( current_play.enemy_field, enemyShips );

            // Обновляем отображение
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();
            pictureBox4.Refresh();
            pictureBox5.Refresh();
            pictureBox6.Refresh();

            MessageBox.Show( "Этап расстановки" );
        }

        private void pictureBox1_Paint( object sender, PaintEventArgs e ) // Отрисовка поля игрока
        {
            if ( current_play != null )
            {
                Graphics g = e.Graphics;

                for ( int i = 0; i < 10; i++ )
                {
                    for ( int j = 0; j < 10; j++ )
                    {
                        Rectangle rec = new Rectangle( i * current_play.cellW, j * current_play.cellH, current_play.cellW, current_play.cellH );
                        g.DrawRectangle( Pens.DarkBlue, rec );

                        switch ( current_play.user_field[ i, j ] )
                        {
                            case CellType.OpenHit:
                                g.FillRectangle( Brushes.Red, rec );
                                break;
                            case CellType.OpenNull:
                                g.FillRectangle( Brushes.Transparent, rec );
                                break;
                            case CellType.OpenLife:
                                g.FillRectangle( Brushes.Green, rec );
                                break;
                        }
                    }
                }
            }
        }

        private void pictureBox2_Paint( object sender, PaintEventArgs e ) // Отрисовка поля противника
        {
            if ( current_play != null )
            {
                Graphics g = e.Graphics;

                for ( int i = 0; i < 10; i++ )
                {
                    for ( int j = 0; j < 10; j++ )
                    {
                        Rectangle rec = new Rectangle( i * current_play.cellWEnemy, j * current_play.cellHEnemy, current_play.cellWEnemy, current_play.cellHEnemy );
                        g.DrawRectangle( Pens.Red, rec );

                        switch ( current_play.enemy_field[ i, j ] )
                        {
                            case CellType.CloseNull:
                                g.FillRectangle( Brushes.Transparent, rec );
                                break;
                            case CellType.OpenHit:
                                g.FillRectangle( Brushes.Red, rec );
                                break;
                            case CellType.OpenLoss:
                                g.FillRectangle( Brushes.Blue, rec );
                                break;
                            case CellType.CloseLife:
                                g.FillRectangle( Brushes.Gray, rec );
                                break;
                        }
                    }
                }
            }
        }

        private void pictureBox2_MouseDown( object sender, MouseEventArgs e ) // Логика атаки игрока
        {
            if ( current_play != null )
            {
                int x = e.X / current_play.cellWEnemy;
                int y = e.Y / current_play.cellHEnemy;

                if ( x >= 0 && x < 10 && y >= 0 && y < 10 )
                {
                    bool result = current_play.TernUser( x * current_play.cellWEnemy, y * current_play.cellHEnemy );

                    if ( result )
                    {
                        pictureBox2.Refresh();

                        if ( current_play.IsPlayerWin() )
                        {
                            MessageBox.Show( "Вы победили!" );
                        }
                        else
                        {
                            EnemyTurn();
                        }
                    }
                }
            }
        }

        private void EnemyTurn() // Логика хода противника
        {
            Random rnd = new Random();
            int x, y;

            do
            {
                x = rnd.Next( 0, 10 );
                y = rnd.Next( 0, 10 );
            } while ( current_play.user_field[ x, y ] == CellType.OpenHit || current_play.user_field[ x, y ] == CellType.OpenLoss );

            current_play.TernEnemy( x * current_play.cellW, y * current_play.cellH );
            pictureBox1.Refresh();

            if ( current_play.IsEnemyWin() )
            {
                MessageBox.Show( "Вы проиграли!" );
            }
        }

        private void PaintShip( int length, PaintEventArgs e, PictureBox pictureBox ) // Отрисовка кораблей
        {
            if ( ships != null )
            {
                Graphics g = e.Graphics;
                int yOffset = 10; // Отступ сверху

                foreach ( var ship in ships )
                {
                    if ( ship.Length == length )
                    {
                        for ( int i = 0; i < ship.Length; i++ )
                        {
                            int x = 10 + ( ship.IsHorizontal ? i * current_play.cellW : 0 );
                            int y = yOffset + ( ship.IsHorizontal ? 0 : i * current_play.cellH );

                            Rectangle rec = new Rectangle( x, y, current_play.cellW, current_play.cellH );
                            g.DrawRectangle( Pens.DarkBlue, rec );
                            g.FillRectangle( Brushes.Green, rec );
                        }

                        yOffset += 50; // Отступ между кораблями
                    }
                }
            }
        }

        private void StartDrag( object sender, MouseEventArgs e, int length ) // Общий метод для начала перетаскивания
        {
            if ( ships != null )
            {
                foreach ( var ship in ships )
                {
                    if ( ship.Length == length )
                    {
                        ( ( PictureBox )sender ).DoDragDrop( ship, DragDropEffects.Move );
                        return;
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
                    if ( field[ startX + i, startY ] != CellType.OpenNull )
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
                    if ( field[ startX, startY + i ] != CellType.OpenNull )
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        //отрисовка кораблей в формах для перетаскивания
        private void pictureBox3_Paint( object sender, PaintEventArgs e )
        {
            PaintShip( 1, e, pictureBox3 );
        }

        private void pictureBox4_Paint( object sender, PaintEventArgs e )
        {
            PaintShip( 2, e, pictureBox4 );
        }

        private void pictureBox6_Paint( object sender, PaintEventArgs e )
        {
            PaintShip( 3, e, pictureBox5 );
        }

        private void pictureBox5_Paint( object sender, PaintEventArgs e )
        {
            PaintShip( 4, e, pictureBox6 );
        }

        private void pictureBox3_MouseDown( object sender, MouseEventArgs e )// Начало перетаскивания однопалубных кораблей
        {
            StartDrag( sender, e, 1 );
        }

        private void pictureBox4_MouseDown( object sender, MouseEventArgs e )// Начало перетаскивания двупалубных кораблей
        {
            StartDrag( sender, e, 2 );
        }

        private void pictureBox6_MouseDown( object sender, MouseEventArgs e )// Начало перетаскивания трехпалубных кораблей
        {
            StartDrag( sender, e, 3 );
        }

        private void pictureBox5_MouseDown( object sender, MouseEventArgs e )// Начало перетаскивания четырехпалубных кораблей
        {
            StartDrag( sender, e, 4 );
        }

        private void pictureBox1_DragEnter( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( typeof( Ship ) ) )
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox1_DragDrop( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( typeof( Ship ) ) )
            {
                Ship ship = ( Ship )e.Data.GetData( typeof( Ship ) );

                Point dropLocation = pictureBox1.PointToClient( new Point( e.X, e.Y ) );
                int startX = dropLocation.X / current_play.cellW;
                int startY = dropLocation.Y / current_play.cellH;

                if ( CanPlaceShip( startX, startY, ship.Length, ship.IsHorizontal, current_play.user_field ) )
                {
                    if ( ship.IsHorizontal )
                    {
                        for ( int i = 0; i < ship.Length; i++ )
                        {
                            current_play.user_field[ startX + i, startY ] = CellType.OpenLife;
                        }
                    }
                    else
                    {
                        for ( int i = 0; i < ship.Length; i++ )
                        {
                            current_play.user_field[ startX, startY + i ] = CellType.OpenLife;
                        }
                    }

                    ships.Remove( ship ); // Удаляем корабль из списка доступных
                    pictureBox1.Refresh(); // Обновляем поле

                    // Обновляем отображение кораблей
                    pictureBox3.Refresh();
                    pictureBox4.Refresh();
                    pictureBox5.Refresh();
                    pictureBox6.Refresh();
                }
            }
        }
    }
}