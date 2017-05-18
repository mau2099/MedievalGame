using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Constantes
{
}

public class MedievalObjects
{
    public const string goArcherName = "MG_Archer";
    public const string goBattlefield = "Battlefield";
}

public class MedievalCharacter
{
    /// <summary>
    /// Characteristics
    /// </summary>
    public struct Properties
    {
        private string _tagGameObject;
        private float _hp;
        private int _pm;
        private float _attackDistance;
        private float _attackForce;
        private int _coolDownMove;
        private int _coolDownAttack;

        /// <summary>
        /// Hit Points of Character
        /// </summary>
        public float Hp
        {
            get
            {
                return _hp;
            }

            set
            {
                _hp = value;
            }
        }

        /// <summary>
        /// Tag Type of Character
        /// </summary>
        public string TagGameObject
        {
            get
            {
                return _tagGameObject;
            }

            set
            {
                _tagGameObject = value;
            }
        }

        /// <summary>
        /// Movement Points
        /// </summary>
        public int Pm
        {
            get
            {
                return _pm;
            }

            set
            {
                _pm = value;
            }
        }

        /// <summary>
        /// Distance for attacking
        /// </summary>
        public float AttackDistance
        {
            get
            {
                return _attackDistance;
            }

            set
            {
                _attackDistance = value;
            }
        }

        /// <summary>
        /// Attack Force. It relies on how good a finger gesture is done
        /// </summary>
        public float AttackForce
        {
            get
            {
                return _attackForce;
            }

            set
            {
                _attackForce = value;
            }
        }

        /// <summary>
        /// Cost in turns for only moving the character
        /// </summary>
        public int CoolDownMove
        {
            get
            {
                return _coolDownMove;
            }

            set
            {
                _coolDownMove = value;
            }
        }

        /// <summary>
        /// Cost in turnos for only attack with the haracter
        /// </summary>
        public int CoolDownAttack
        {
            get
            {
                return _coolDownAttack;
            }

            set
            {
                _coolDownAttack = value;
            }
        }

        public List<Node> pathToFollow { get; set; }
    }
    public Properties Charcs;
}