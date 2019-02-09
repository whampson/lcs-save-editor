#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Infrastructure;
using System.ComponentModel;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Represents a garage object.
    /// </summary>
    public abstract class Garage : SerializableObject
    {
        protected GarageType m_type;
        protected GarageState m_state;
        protected byte m_maxCarsAllowed;
        protected uint m_doorObjectPointer;
        protected bool m_rotatingDoor;
        protected bool m_specialCamera;
        protected Vector3d m_location;
        protected Quaternion m_rotation;
        protected float m_ceilingZ;
        protected float m_doorCurrentHeight;
        protected float m_doorMaxHeight;
        protected float m_x1;
        protected float m_x2;
        protected float m_y1;
        protected float m_y2;
        protected float m_doorX;
        protected float m_doorY;
        protected float m_doorZ;
        protected uint m_timer;

        public GarageType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public GarageState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public byte MaxCarsAllowed
        {
            get { return m_maxCarsAllowed; }
            set { m_maxCarsAllowed = value; OnPropertyChanged(); }
        }

        public uint DoorObjectPointer
        {
            get { return m_doorObjectPointer; }
            set { m_doorObjectPointer = value; OnPropertyChanged(); }
        }

        public bool HasRotatingDoor
        {
            get { return m_rotatingDoor; }
            set { m_rotatingDoor = value; OnPropertyChanged(); }
        }

        public bool HasSpecialCamera
        {
            get { return m_specialCamera; }
            set { m_specialCamera = value; OnPropertyChanged(); }
        }

        public Vector3d Location
        {
            get { return m_location; }
            set {
                if (m_location != null) {
                    m_location.PropertyChanged -= Location_PropertyChanged;
                }
                m_location = value;
                m_location.PropertyChanged += Location_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public Quaternion Rotation
        {
            get { return m_rotation; }
            set {
                if (m_rotation != null) {
                    m_rotation.PropertyChanged -= Rotation_PropertyChanged;
                }
                m_rotation = value;
                m_rotation.PropertyChanged += Rotation_PropertyChanged;
                OnPropertyChanged();
            }
        }

        public float CeilingZ
        {
            get { return m_ceilingZ; }
            set { m_ceilingZ = value; OnPropertyChanged(); }
        }

        public float DoorCurrentHeight
        {
            get { return m_doorCurrentHeight; }
            set { m_doorCurrentHeight = value; OnPropertyChanged(); }
        }

        public float DoorMaxHeight
        {
            get { return m_doorMaxHeight; }
            set { m_doorMaxHeight = value; OnPropertyChanged(); }
        }

        public float X1
        {
            get { return m_x1; }
            set { m_x1 = value; OnPropertyChanged(); }
        }

        public float X2
        {
            get { return m_x2; }
            set { m_x2 = value; OnPropertyChanged(); }
        }

        public float Y1
        {
            get { return m_y1; }
            set { m_y1 = value; OnPropertyChanged(); }
        }

        public float Y2
        {
            get { return m_y2; }
            set { m_y2 = value; OnPropertyChanged(); }
        }

        public float DoorX
        {
            get { return m_doorX; }
            set { m_doorX = value; OnPropertyChanged(); }
        }

        public float DoorY
        {
            get { return m_doorY; }
            set { m_doorY = value; OnPropertyChanged(); }
        }

        public float DoorZ
        {
            get { return m_doorZ; }
            set { m_doorZ = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}, {2} = {3}, {4} = {5}",
                nameof(Type), Type,
                nameof(Location), Location,
                nameof(Timer), Timer);
        }

        private void Location_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Location));
        }

        private void Rotation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Rotation));
        }
    }
}
