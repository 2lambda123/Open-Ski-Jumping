using System;
using System.Linq;
using OpenSkiJumping.Competition.Persistent;
using OpenSkiJumping.Data;
using OpenSkiJumping.UI.CalendarEditor.Classifications;
using OpenSkiJumping.UI.CalendarEditor.Competitors;
using OpenSkiJumping.UI.CalendarEditor.Events;
using OpenSkiJumping.UI.CalendarsMenu;
using UnityEngine;

namespace OpenSkiJumping.UI.CalendarEditor
{
    public class CalendarEditorController : MonoBehaviour
    {
        private Calendar calendar;
        [SerializeField] private CalendarFactory calendarFactory;
        [SerializeField] private CalendarsMenuView calendarsMenu;
        [SerializeField] private CalendarEditorClassificationsView classificationsMenu;
        [SerializeField] private CompetitorsRuntime competitorsRuntime;
        [SerializeField] private CalendarEditorEventsView eventsMenu;

        [SerializeField] private CalendarEditorJumpersView jumpersMenu;
        [SerializeField] private GameObject contentGameObject;
        

        private void LoadCalendar(Calendar calendarToLoad)
        {
            calendarFactory.Competitors = competitorsRuntime.GetJumpersById(calendarToLoad.competitorsIds).ToList();
            calendarFactory.LoadClassifications(calendarToLoad.classifications);
            calendarFactory.Events = calendarToLoad.events;
        }

        private void SaveToCalendar(Calendar calendarToSave)
        {
            var newCalendar = calendarFactory.CreateCalendar();
            calendarToSave.competitorsIds = newCalendar.competitorsIds;
            calendarToSave.classifications = newCalendar.classifications;
            calendarToSave.teams = newCalendar.teams;
            calendarToSave.events = newCalendar.events;
        }

        public void LoadCalendarEditor()
        {
            if (calendarsMenu.SelectedCalendar == null) return;
            calendar = calendarsMenu.SelectedCalendar;
            LoadCalendar(calendar);
            calendarsMenu.gameObject.SetActive(false);
            contentGameObject.SetActive(true);
        }

        public void CloseCalendarEditor()
        {
            if (calendar == null) return;
            jumpersMenu.SelectionSave();

            SaveToCalendar(calendar);
            contentGameObject.SetActive(false);
            calendarsMenu.gameObject.SetActive(true);
        }

        public void LoadJumpersMenu()
        {
            jumpersMenu.gameObject.SetActive(true);
            classificationsMenu.gameObject.SetActive(false);
            eventsMenu.gameObject.SetActive(false);
        }

        public void LoadClassificationsMenu()
        {
            jumpersMenu.gameObject.SetActive(false);
            classificationsMenu.gameObject.SetActive(true);
            eventsMenu.gameObject.SetActive(false);
        }

        public void LoadEventsMenu()
        {
            calendarFactory.RecalculateEvents();
            jumpersMenu.gameObject.SetActive(false);
            classificationsMenu.gameObject.SetActive(false);
            eventsMenu.gameObject.SetActive(true);
        }
        
    }
}