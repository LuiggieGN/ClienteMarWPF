using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Base
{
    public abstract class DataKeyBase<TId> :IDataKeyBase<TId>
    {

        public virtual TId Id { get;  set; }

        int? _requestedHashCode;

        public bool IsTransient()
        {
            return Id.Equals(default(TId));
        }

        public override bool Equals(object objeto)
        {
            if (objeto == null || !(objeto is DataKeyBase<TId>))
            {
                return false;
            }

            if (ReferenceEquals(this, objeto))
            {
                return true;
            }

            if (GetType() != objeto.GetType())
            {
                return false;
            }

            var objetoEntranteCasteado = (DataKeyBase<TId>)objeto;

            if (objetoEntranteCasteado.IsTransient() || IsTransient())
            {
                return false;
            }
            else
            {
                return objetoEntranteCasteado == this;

            }

        }//Fin de metodo Equals( )


        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 31;
                }

                return _requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }// Fin de metodo GetHashCode( )

        public static bool operator ==(DataKeyBase<TId> left, DataKeyBase<TId> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null) ? true : false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        public static bool operator !=(DataKeyBase<TId> left, DataKeyBase<TId> right)
        {
            return !(left == right);
        }


    }
}
