using NCrontab;

namespace JewelryApp.Business.Jobs;

public sealed record CronRegistryEntry(Type Type, CrontabSchedule CronTabSchedule);