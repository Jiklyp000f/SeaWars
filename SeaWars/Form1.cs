using System;
using System.Drawing;
using System.Windows.Forms;

/*
 РЕАЛИЗОВАТЬ:
1. методы отрисовки кораблей, полей и тд...
2. логику попадания:
    1. после уничтожения закрывать/ открывать клетки вокруг пораженной цели
    2. логика нажатия на поле(после идёт проверка на наполненность ячейки)
3. ИИ для противника(простенький)
 */

namespace SeaWars
{
    public partial class Form1 : Form
    {
        playseabattle current_play = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e ) // функция обработки нажатия на поле
        {

        }

        private void pictureBox1_Paint( object sender, PaintEventArgs e ) // отрисовка поля игрока
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
                            default:
                                break;
                        }
                    }
                }


            }
        }

        private void pictureBox1_MouseDown( object sender, MouseEventArgs e ) // нажатия на поле
        {

            if ( e.Button == MouseButtons.Left )
            {
                current_play.putShip( e.X, e.Y );
            }
            else
            {
                current_play.delShip( e.X, e.Y );
            }

            pictureBox1.Refresh();
        }

        private void pictureBox2_Paint( object sender, PaintEventArgs e ) // отрисовка поля врага
        {

            if ( current_play != null )
            {
                Graphics g = e.Graphics;

                for ( int i = 0; i < 10; i++ )
                {
                    for ( int j = 0; j < 10; j++ )
                    {
                        Rectangle rec = new Rectangle( i * current_play.cellWEnemy, j * current_play.cellHEnemy, current_play.cellWEnemy, current_play.cellHEnemy ); // отрисовка поля по x, y
                        g.DrawRectangle( Pens.Red, rec );
                        switch ( current_play.enemy_field[ i, j ] )
                        {
                            case CellType.CloseNull:
                                g.DrawRectangle( Pens.Black, rec );
                                break;
                            case CellType.OpenHit:
                                g.FillRectangle( Brushes.Red, rec );
                                break;
                            case CellType.OpenLoss:
                                g.FillRectangle( Brushes.Blue, rec );
                                break;
                            case CellType.CloseLife:
                                g.DrawRectangle( Pens.Black, rec );
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void pictureBox2_Click( object sender, EventArgs e )
        {
            //current_play.TernEnemy( e.X, e.Y );

        }

        private void button2_Click( object sender, EventArgs e )
        {
            if ( current_play != null )
            {
                current_play.autoPutShips();
                pictureBox2.Refresh();
            }
        }

        private void pictureBox2_MouseDown( object sender, MouseEventArgs e )
        {
            if ( current_play != null )
            {
                current_play.TernUser( e.X, e.Y );
                pictureBox2.Refresh();
            }
        }

        private void новаяToolStripMenuItem_Click( object sender, EventArgs e )
        {
            current_play = new playseabattle();
            current_play.Init( pictureBox1.Width, pictureBox1.Height, pictureBox2.Width, pictureBox2.Height );

            pictureBox1.Refresh(); // методы обновления поля
            pictureBox2.Refresh();
            pictureBox3.Refresh();
            pictureBox4.Refresh();
            pictureBox5.Refresh();
            pictureBox6.Refresh();
            if ( playseabattle.Turn == 0 )
            {
                MessageBox.Show( "Этап расстановки" );
            }
            else
            { MessageBox.Show( $"Ход: {playseabattle.Turn.ToString()}" ); }
        }

        private void pictureBox1_MouseMove( object sender, MouseEventArgs e )
        {
            if ( current_play != null )
            {
                int x = e.X / current_play.cellW;
                int y = e.Y / current_play.cellH; // логика отрисовки врага
                Text = $"({x};{y})";
            }

        }
        public void PaintShip( int Length, PaintEventArgs e ) //отрисовка кораблей
        {
            if ( current_play != null )
            {
                Graphics g = e.Graphics;

                for ( int i = 0; i < Length; i++ )
                {
                    for ( int j = 0; j < 1; j++ )
                    {
                        Rectangle rec = new Rectangle( i * current_play.cellWEnemy + i, j * current_play.cellHEnemy + j, current_play.cellWEnemy, current_play.cellHEnemy ); // отрисовка поля по x, y
                        g.DrawRectangle( Pens.DarkBlue, rec );
                        g.FillRectangle( Brushes.Green, rec );
                    }
                }
            }
        }
        private void pictureBox3_Paint( object sender, PaintEventArgs e ) //однопалубные
        {
            PaintShip( 1, e );
        }

        private void pictureBox4_Paint( object sender, PaintEventArgs e )//двупалубные
        {
            PaintShip( 2, e );
        }

        private void pictureBox6_Paint( object sender, PaintEventArgs e )//трехпалубные
        {
            PaintShip( 3, e );
        }

        private void pictureBox5_Paint( object sender, PaintEventArgs e )//четырёхпалубные
        {
            PaintShip( 4, e );
        }
    }
}
