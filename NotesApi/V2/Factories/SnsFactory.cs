using Hackney.Core.JWT;
using Hackney.Core.Sns;
using NotesApi.V2.Domain;
using NotesApi.V2.Infrastructure.JWT;
using System;

namespace NotesApi.V2.Factories
{
    public class SnsFactory : ISnsFactory
    {
        public EntityEventSns NoteCreated(Note note, Token token)
        {
            var eventSns = new EntityEventSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = note.Id,
                Id = Guid.NewGuid(),
                EventType = NoteCreatedEventConstants.EVENTTYPE,
                Version = NoteCreatedEventConstants.V2_VERSION,
                SourceSystem = NoteCreatedEventConstants.SOURCE_SYSTEM,
                EventData = new EventData
                {
                    NewData = note
                },
                User = new User { Name = token.Name, Email = token.Email }
            };

            switch (note.TargetType)
            {
                case TargetType.person:
                    eventSns.SourceDomain = NoteCreatedEventConstants.PERSON_DOMAIN;
                    break;
                case TargetType.asset:
                    eventSns.SourceDomain = NoteCreatedEventConstants.ASSET_DOMAIN;
                    break;
                case TargetType.tenure:
                    eventSns.SourceDomain = NoteCreatedEventConstants.TENURE_DOMAIN;
                    break;
                case TargetType.repair:
                    eventSns.SourceDomain = NoteCreatedEventConstants.REPAIR_DOMAIN;
                    break;
                case TargetType.process:
                    eventSns.SourceDomain = NoteCreatedEventConstants.PROCESS_DOMAIN;
                    break;
                default:
                    eventSns.SourceDomain = NoteCreatedEventConstants.SOURCE_DOMAIN;
                    break;
            }

            return eventSns;
        }
    }
}
