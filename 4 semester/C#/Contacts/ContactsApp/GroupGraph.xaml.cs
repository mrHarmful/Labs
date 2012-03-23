using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using ContactsLib;
using ContactsLib.Entities;

namespace ContactsApp
{
    public partial class GroupGraph : FrameworkElement
    {
        public static DependencyProperty ContactsProperty = DependencyProperty.Register("Contacts", typeof (ContactList),
                                                                                        typeof (GroupGraph));

        private readonly DrawingVisual visual = new DrawingVisual();

        public GroupGraph()
        {
            InitializeComponent();
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        public ContactList Contacts
        {
            get { return (ContactList) GetValue(ContactsProperty); }
            set { SetValue(ContactsProperty, value); }
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void Update()
        {
            int S = 200, C = S/2, R = S/3;

            DrawingContext ctx = visual.RenderOpen();
            ctx.DrawRectangle(Brushes.White, null, new Rect(0, 0, S, S));

            double ang = 0;
            int total = 0;
            foreach (ContactGroup g in Contacts.Groups)
                total += g.Contacts.Count;

            var type = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal,
                                    FontStretches.Normal);
            int idx = 0;
            foreach (ContactGroup g in Contacts.Groups)
            {
                idx++;
                double ap = 1.0*g.Contacts.Count/total*Math.PI*2;
                var fig = new PathFigure();
                fig.Segments.Add(new LineSegment(new Point(Math.Cos(ang)*R, Math.Sin(ang)*R), true));
                fig.Segments.Add(new ArcSegment(new Point(Math.Cos(ang + ap)*R, Math.Sin(ang + ap)*R), new Size(R, R),
                                                ap, (ap > Math.PI), SweepDirection.Clockwise,
                                                true));
                var l = new List<PathFigure>();
                l.Add(fig);

                var fill =
                    new SolidColorBrush(Color.FromRgb((byte) ((idx*50)%255), (byte) ((idx*50 + 80)%255),
                                                      (byte) ((idx*50 + 160)%255)));
                var stroke = new SolidColorBrush(Color.Multiply(fill.Color, 0.5f));
                ctx.DrawGeometry(fill, new Pen(stroke, 2),
                                 new PathGeometry(l, FillRule.Nonzero, new TranslateTransform(C, C)));
                var tPoint = new Point(C + (float) Math.Cos(ang + ap/2)*R/2, C + (float) Math.Sin(ang + ap/2)*R/2);
                ctx.DrawText(
                    new FormattedText(g.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, type, 12, stroke),
                    tPoint);
                tPoint.X++;
                tPoint.Y--;
                ctx.DrawText(
                    new FormattedText(g.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, type, 12,
                                      Brushes.White), tPoint);
                ang += ap;
            }
            ctx.Close();
        }

        protected override Visual GetVisualChild(int index)
        {
            return visual;
        }
    }
}