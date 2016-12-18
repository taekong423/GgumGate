using UnityEngine;
using UnityEditor;
using System.Collections;

namespace New
{
    [CustomEditor(typeof(Event))]
    public class EventEditor : Editor
    {
        Event _target;

        void OnEnable()
        {
            _target = target as Event;
        }

        public override void OnInspectorGUI()
        {
            _target.Type = (Event_Type)EditorGUILayout.EnumPopup("EventType", _target.Type);

            switch (_target.Type)
            {
                case Event_Type.HealthChange:
                    {
                        _target._healthTarget = (Character)EditorGUILayout.ObjectField("Target", _target._healthTarget, typeof(Character), true);
                        _target._health = EditorGUILayout.FloatField("Health", _target._health);
                    }
                    break;

                case Event_Type.OnDialogue:
                    {
                        _target._dataID = (DialogueID)EditorGUILayout.EnumPopup("DataID", _target._dataID);
                    }
                    break;

                case Event_Type.Teleport:
                    {
                        _target._teleportTarget = (Transform)EditorGUILayout.ObjectField("Target", _target._teleportTarget, typeof(Transform), true);
                        _target._point = (Transform)EditorGUILayout.ObjectField("Point", _target._point, typeof(Transform), true);
                        _target._pointVector = (Vector3)EditorGUILayout.Vector3Field("PointVector", _target._pointVector);
                    }
                    break;


                case Event_Type.Dead:
                    {
                        _target._deadTarget = (Character)EditorGUILayout.ObjectField("Target", _target._deadTarget, typeof(Character), true);
                    }
                    break;

                default:
                    break;
            }
        }
    } 
}
