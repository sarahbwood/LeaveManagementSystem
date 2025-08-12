namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    // This enum represents the possible statuses of a leave request.
    // It is used to track the state of each leave request in the system.
    // The values correspond to different stages in the leave request lifecycle.

    public enum LeaveRequestStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Declined = 3,
        Cancelled = 4
    }


}
