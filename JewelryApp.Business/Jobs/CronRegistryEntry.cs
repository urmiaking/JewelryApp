using NCrontab;

namespace JewelryApp.Application.Jobs;

public sealed record CronRegistryEntry(Type Type, CrontabSchedule CronTabSchedule);