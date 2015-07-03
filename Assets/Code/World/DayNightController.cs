using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Assets.Code.World
{

    public class DayNightController : MonoBehaviour
    {

        public Light Source;
        //public Light Moon;
        public float SecondsInFullDay = 120f;
        [Range(0, 1)]
        public float CurrentTimeOfDay = 0;
        [HideInInspector]
        public float TimeMultiplier = 1f;

        private float _sunInitialIntensity;
        //private float _moonInitialIntensity;

        void Start()
        {
            _sunInitialIntensity = Source.intensity;
            //_moonInitialIntensity = Moon.intensity;
        }

        void Update()
        {
            UpdateSun();

            CurrentTimeOfDay += (Time.deltaTime / SecondsInFullDay) * TimeMultiplier;

            if (CurrentTimeOfDay >= 1)
            {
                CurrentTimeOfDay = 0;
            }
        }

        void UpdateSun()
        {
            Source.transform.localRotation = Quaternion.Euler((CurrentTimeOfDay * 360f) - 90, 170, 0);

            float intensityMultiplier = 1;
            if (CurrentTimeOfDay <= 0.23f || CurrentTimeOfDay >= 0.75f)
            {
                intensityMultiplier = 0;
            }
            else if (CurrentTimeOfDay <= 0.25f)
            {
                intensityMultiplier = Mathf.Clamp01((CurrentTimeOfDay - 0.23f) * (1 / 0.02f));
            }
            else if (CurrentTimeOfDay >= 0.73f)
            {
                intensityMultiplier = Mathf.Clamp01(1 - ((CurrentTimeOfDay - 0.73f) * (1 / 0.02f)));
            }

            Source.intensity = _sunInitialIntensity * intensityMultiplier;
            //Sun.intensity = _moonInitialIntensity * -intensityMultiplier;
        }
    }
}
