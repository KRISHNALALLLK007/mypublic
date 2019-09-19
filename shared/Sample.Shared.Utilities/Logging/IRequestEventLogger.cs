namespace Sample.Shared.Utilities.Logging
{
    public interface IRequestEventLogger
    {
        void LogCourseRequestEvent(CourseRequestEventLog requestEventLog);
    }
}
