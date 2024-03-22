using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Sophia.AI
{
    public class EQS : MonoBehaviour
    {
        [Header("EQS Settings")]
        public float eqsRadius;

        [Range(0f, 360f)]
        public int itemAmount;
        public LayerMask hitMask;

        private float maxDistance = 0;
        List<Vector3> itemList;
        List<Vector3> bestItemList;

        // Start is called before the first frame update
        public Vector3 RunEQS()
        {
            maxDistance = 0;
            itemList = new List<Vector3>();
            bestItemList = new List<Vector3>();

            float angle = 0;

            for (int i = 0; i < itemAmount; i++)
            {
                float x = Mathf.Sin(angle);
                float z = Mathf.Cos(angle);
                angle += 2 * Mathf.PI / itemAmount;

                Vector3 item;
                Vector3 dir = new Vector3(transform.position.x + x, 0, transform.position.z + z);
                float distance;
                RaycastHit hit;
                NavMeshHit navHit;

                if (Physics.Raycast(transform.position, dir, out hit, eqsRadius, hitMask))
                {
                    item = hit.point;
                    distance = hit.distance;
                }
                else
                {
                    item = dir * eqsRadius;
                    distance = Vector3.Distance(transform.position, dir * eqsRadius);
                }

                if (NavMesh.SamplePosition(item, out navHit, eqsRadius, NavMesh.AllAreas))
                {
                    itemList.Add(item);
                    if (distance > maxDistance)
                        maxDistance = distance;
                }
                else
                    itemList.Add(transform.position);
            }

            foreach (Vector3 item in itemList)
            {
                if (Vector3.Distance(transform.position, item) >= maxDistance)
                    bestItemList.Add(item);
            }

            Debug.Log(bestItemList.Count);
            System.Random random = new System.Random();
            return bestItemList[random.Next(0, bestItemList.Count)];
        }
        void Start()
        {
            itemList = new List<Vector3>();
            bestItemList = new List<Vector3>();
        }
    }
}