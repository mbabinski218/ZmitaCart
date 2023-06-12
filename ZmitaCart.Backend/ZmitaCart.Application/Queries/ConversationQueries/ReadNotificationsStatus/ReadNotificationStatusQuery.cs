using MediatR;

namespace ZmitaCart.Application.Queries.ConversationQueries.ReadNotificationsStatus;

public record ReadNotificationStatusQuery(int UserId) : IRequest<int>;