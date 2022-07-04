using Hackney.Core.JWT;
using Hackney.Core.Sns;
using NotesApi.V2.Domain;
using NotesApi.V2.Infrastructure.JWT;
using System;
using System.ComponentModel;

namespace NotesApi.V2.Factories
{
    public class SnsFactory : ISnsFactory
    {
        private string GetEventType(Note note)
        {
            switch (note.TargetType)
            {
                case TargetType.person:
                    return NoteCreatedEventConstants.PERSON_NOTE_EVENT;
                case TargetType.asset:
                    return NoteCreatedEventConstants.ASSET_NOTE_EVENT;
                case TargetType.tenure:
                    return NoteCreatedEventConstants.TENURE_NOTE_EVENT;
                case TargetType.repair:
                    return NoteCreatedEventConstants.REPAIR_NOTE_EVENT;
                case TargetType.process:
                    return NoteCreatedEventConstants.PROCESS_NOTE_EVENT;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        public EntityEventSns NoteCreated(Note note, Token token)
        {
            var eventSns = new EntityEventSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = note.TargetId,
                Id = Guid.NewGuid(),
                EventType = GetEventType(note),
                Version = NoteCreatedEventConstants.V2_VERSION,
                SourceSystem = NoteCreatedEventConstants.SOURCE_SYSTEM,
                SourceDomain = NoteCreatedEventConstants.SOURCE_DOMAIN,
                EventData = new EventData
                {
                    NewData = note
                },
                User = new User { Name = token.Name, Email = token.Email }
            };

            return eventSns;
        }
    }
}
