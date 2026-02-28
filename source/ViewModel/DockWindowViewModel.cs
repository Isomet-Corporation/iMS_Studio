using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iMS;
using System.Linq.Expressions;

namespace iMS_Studio.ViewModel
{
    public abstract class DockWindowViewModel : BaseViewModel
    {
        #region Properties

        private bool _IsPlaying;
        public bool IsPlaying {
            get { return _IsPlaying; }
            set
            {
                if (_IsPlaying != value)
                {
                    _IsPlaying = value;
                    OnPropertyChanged("IsPlaying");
                }
            }
        }

        #region CloseCommand
        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new RelayCommand(call => Close());
                return _CloseCommand;
            }
        }
        #endregion

        #region IsClosed
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged(nameof(IsClosed));
                }
            }
        }
        #endregion

        #region CanClose
        private bool _CanClose;
        public bool CanClose
        {
            get { return _CanClose; }
            set
            {
                if (_CanClose != value)
                {
                    _CanClose = value;
                    OnPropertyChanged(nameof(CanClose));
                }
            }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        #endregion

        #region IsVisible
        public bool IsVisible
        {
            get { return true; }
            set { }
        }
        #endregion

        #region Title
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                ApplyPropertyChange<DockWindowViewModel, string>(ref _Title, o => o.Title, value);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Is the object dirty or not?
        /// </summary>
        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            protected set
            {
                _isDirty = value;
                OnPropertyChanged("IsDirty");
            } 
        }

        public DockWindowViewModel()
        {
            this.CanClose = true;
            this.IsClosed = false;
            this._IsPlaying = false;
            this._isDirty = false;
        }

        public void Close()
        {
            this.IsClosed = true;
        }

        /// <summary>
        /// Reset the object to non-dirty
        /// </summary>
        public void ResetDirtyFlag()
        {
            this.IsDirty = false;
        }
        /// <summary>
        /// Change the property if required and throw event
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void ApplyPropertyChange<T, F>(ref F field,
                    Expression<Func<T, object>> property, F value)
        {
            // Only do this if the value changes
            if (field == null || !field.Equals(value))
            {
                // Get the property
                var propertyExpression = GetMemberExpression(property);
                if (propertyExpression == null)
                    throw new InvalidOperationException("You must specify a property");
                // Property name
                string propertyName = propertyExpression.Member.Name;
                // Set the value
                field = value;
                // set flag
                this.IsDirty = true;
                // Notify change
                OnPropertyChanged(propertyName);
            }
        }
        /// <summary>
        /// Get member expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public MemberExpression GetMemberExpression<T>(Expression<Func<T,
                                object>> expression)
        {
            // Default expression
            MemberExpression memberExpression = null;
            // Convert
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            // Member access
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }
            // Not a member access
            if (memberExpression == null)
                throw new ArgumentException("Not a member access",
                                            "expression");
            // Return the member expression
            return memberExpression;
        }
    }
}
