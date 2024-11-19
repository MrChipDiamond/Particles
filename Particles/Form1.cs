using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Particles
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private List<Particle> particles = new List<Particle>();
        private Timer timer = new Timer();
        private Timer colorTimer = new Timer();
        private int red = 255, green = 0, blue = 0; // Initial RGB values
        private int colorDirection = 1; // To control color cycling

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Initialize particles
            for (int i = 0; i < 5000; i++)
            {
                particles.Add(new Particle
                {
                    Position = new PointF(this.ClientSize.Width / 2, this.ClientSize.Height / 2),
                    Velocity = new PointF((float)(new Random().NextDouble() - 0.5) * 5,
                                          (float)(new Random().NextDouble() - 0.5) * 5),
                    Color = Color.FromArgb(150, red, green, blue), // Use initial RGB values
                    Size = new Random().Next(2, 5)
                });
            }

            // Particle update timer
            timer.Interval = 16; // Approximately 60 FPS
            timer.Tick += (s, e) =>
            {
                UpdateParticles();
                this.Invalidate();
            };
            timer.Start();

            // Color animation timer
            colorTimer.Interval = 50; // Adjust for slower or faster color changes
            colorTimer.Tick += (s, e) =>
            {
                UpdateRGBColors();
                UpdateParticleColors();
            };
            colorTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var particle in particles)
            {
                using (Brush brush = new SolidBrush(particle.Color))
                {
                    e.Graphics.FillEllipse(brush, particle.Position.X, particle.Position.Y, particle.Size, particle.Size);
                }
            }
        }

        public class Particle
        {
            public PointF Position { get; set; }
            public PointF Velocity { get; set; }
            public Color Color { get; set; }
            public int Size { get; set; }
        }

        private void UpdateParticles()
        {
            var rand = new Random();
            foreach (var particle in particles)
            {
                particle.Position = new PointF(particle.Position.X + particle.Velocity.X,
                                               particle.Position.Y + particle.Velocity.Y);

                if (particle.Position.X < 0 || particle.Position.X > this.ClientSize.Width)
                    particle.Velocity = new PointF(-particle.Velocity.X, particle.Velocity.Y);
                if (particle.Position.Y < 0 || particle.Position.Y > this.ClientSize.Height)
                    particle.Velocity = new PointF(particle.Velocity.X, -particle.Velocity.Y);

                particle.Velocity = new PointF(
                    particle.Velocity.X + (float)(rand.NextDouble() - 0.5) * 0.1f,
                    particle.Velocity.Y + (float)(rand.NextDouble() - 0.5) * 0.1f);
            }
        }

        private void UpdateRGBColors()
        {
            // Gradually change RGB values
            if (colorDirection == 1) // Red to Green
            {
                green += 5;
                if (green >= 255)
                {
                    green = 255;
                    colorDirection = 2;
                }
            }
            else if (colorDirection == 2) // Green to Blue
            {
                red -= 5;
                if (red <= 0)
                {
                    red = 0;
                    colorDirection = 3;
                }
            }
            else if (colorDirection == 3) // Blue to Red
            {
                blue += 5;
                if (blue >= 255)
                {
                    blue = 255;
                    colorDirection = 4;
                }
            }
            else if (colorDirection == 4) // Red increasing, Blue decreasing
            {
                green -= 5;
                if (green <= 0)
                {
                    green = 0;
                    colorDirection = 1;
                }
            }
        }

        private void UpdateParticleColors()
        {
            foreach (var particle in particles)
            {
                particle.Color = Color.FromArgb(150, red, green, blue);
            }
        }
    }
}
