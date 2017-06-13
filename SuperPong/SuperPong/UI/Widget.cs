using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.UI
{
    public abstract class Widget : IEventListener
    {
        Widget _parent = null;
        public Widget Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                _parent = value;
                ComputeProperties();
            }
        }

        public Origin Origin
        {
            get;
            protected set;
        }

        public Vector2 TopLeft
        {
            get;
            private set;
        } = Vector2.Zero;
        public Vector2 BottomRight
        {
            get;
            private set;
        } = Vector2.Zero;

        public float Width
        {
            get
            {
                return BottomRight.X - TopLeft.X;
            }
        }
        public float Height
        {
            get
            {
                return BottomRight.Y - TopLeft.Y;
            }
        }

        public float AspectRatio
        {
            get;
            protected set;
        }
        public AspectRatioType AspectRatioType
        {
            get;
            private set;
        }

        protected float _percentX;
        protected float _percentY;
        protected float _pOffsetX;
        protected float _pOffsetY;

        protected float _percentWidth;
        protected float _pOffsetWidth;
        protected float _percentHeight;
        protected float _pOffsetHeight;

        public Widget(Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentWidth,
                      float pOffsetWidth,
                      float percentHeight,
                      float pOffsetHeight)
        {
            Origin = origin;
            _percentX = percentX;
            _percentY = percentY;
            _pOffsetX = pOffsetX;
            _pOffsetY = pOffsetY;
            _percentWidth = percentWidth;
            _pOffsetWidth = pOffsetWidth;
            _percentHeight = percentHeight;
            _pOffsetHeight = pOffsetHeight;

            AspectRatioType = AspectRatioType.None;
        }

        public Widget(Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentAspect,
                      float pOffsetAspect,
                      float aspectRatio,
                      AspectRatioType aspectRatioType)
        {
            Origin = origin;
            _percentX = percentX;
            _percentY = percentY;
            _pOffsetX = pOffsetX;
            _pOffsetY = pOffsetY;

            _percentWidth = percentAspect;
            _pOffsetWidth = pOffsetAspect;
            _percentHeight = percentAspect;
            _pOffsetHeight = pOffsetAspect;

            AspectRatio = aspectRatio;

            AspectRatioType = aspectRatioType;
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public void ComputeProperties()
        {
            if (Parent == null)
            {
                // Root; only sets based on offset width and height
                TopLeft = Vector2.Zero;
                BottomRight = new Vector2(_pOffsetWidth, _pOffsetHeight);
            }
            else
            {
                float x = Parent.TopLeft.X;
                x += _percentX * Parent.Width;
                x += _pOffsetX;

                float y = Parent.TopLeft.Y;
                y += _percentY * Parent.Height;
                y += _pOffsetY;

                float w, h;
                switch (AspectRatioType)
                {
                    case AspectRatioType.WidthMaster:
                        w = Parent.Width;
                        w *= _percentWidth;
                        w += _pOffsetWidth;

                        h = w / AspectRatio;
                        break;
                    case AspectRatioType.HeightMaster:
                        h = Parent.Height;
                        h *= _percentHeight;
                        h += _pOffsetHeight;

                        w = h * AspectRatio;
                        break;
                    case AspectRatioType.None:
                    default:
                        w = Parent.Width;
                        w *= _percentWidth;
                        w += _pOffsetWidth;
                        h = Parent.Height;
                        h *= _percentHeight;
                        h += _pOffsetHeight;

                        AspectRatio = w / h;
                        break;
                }

                switch (Origin)
                {
                    case Origin.BottomRight:
                    case Origin.BottomLeft:
                        y = Parent.BottomRight.Y - (y - Parent.TopLeft.Y);
                        y -= h;

                        if (Origin == Origin.BottomRight)
                        {
                            goto case Origin.TopRight;
                        }
                        break;
                    case Origin.TopRight:
                        x = Parent.BottomRight.X - (x - Parent.TopLeft.X);
                        x -= w;
                        break;
                    case Origin.Center:
                        x = x + Parent.Width * 0.5f;
                        x -= (w * 0.5f);

                        y = y + Parent.Height * 0.5f;
                        y -= (h * 0.5f);
                        break;
                }

                TopLeft = new Vector2(x, y);
                BottomRight = new Vector2(x + w,
                                          y + h);
            }

            OnComputeProperties();
        }

        protected virtual void OnComputeProperties()
        {
        }

        public abstract bool Handle(IEvent evt);
    }

    public enum Origin
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }

    public enum AspectRatioType
    {
        None,
        WidthMaster,
        HeightMaster
    }
}
