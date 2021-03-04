using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;

using RPG.Combat;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    public class MousePlayerController : MonoBehaviour
    {
        Mover _Mover;
        Fighter _Fighter;
        // Start is called before the first frame update
        void Start()
        {
            _Mover = this.GetComponent<Mover>();
            _Fighter = this.GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithCombat())
            {
                return;
            }
            if (IntercatWithMovement())
            {
                return;
            }
            
        }

        private bool InteractWithCombat()
        {
          RaycastHit[] hits =  Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<Combat.CombatTarget>();
                if (target == null)
                {
                    
                    continue;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    _Fighter.Attack(target);
                    
                }
                 return true;
            }
            return false;
        }

        private bool IntercatWithMovement()
        {
        
              return  MoveToCursor();
          
        }

        bool MoveToCursor()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;

            bool hasHIt = Physics.Raycast(ray, out hit);


            if (hasHIt)
            {
                if (Input.GetMouseButton(0))
                {
                    _Mover.MoveTo(hit.point);
                }
            }

            return hasHIt;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}