//
//  DefaultStatsProvider.m
//  iSanta
//
//  Created by Eric Henderson on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "DefaultStatsProvider.h"
#import "stats.h"

@implementation DefaultStatsProvider

- (NSInteger)getRowCount:(NSMutableArray *)points
{
    return 11;
}

- (NSString *)getTitleForIndex:(NSInteger)index
                    withPoints:(NSMutableArray *)points
{
    if(index == 0)
    {
        return @"Extreme Spread X";
    }
    else if(index == 1)
    {
        return @"Extreme Spread Y";
    }
    else if(index == 2)
    {
        return @"Extreme Spread Group";
    }
    else if(index == 3)
    {
        return @"Center X";
    }
    else if(index == 4)
    {
        return @"Center Y";
    }
    else if(index == 5)
    {
        return @"Standard Deviation X";
    }
    else if(index == 6)
    {
        return @"Standard Deviation Y";
    }
    else if(index == 7)
    {
        return @"Furthest Left";
    }
    else if(index == 8)
    {
        return @"Furthest Right";
    }
    else if(index == 9)
    {
        return @"Highest Round";
    }
    else if(index == 10)
    {
        return @"Lowest Round";
    }
    return @"";
}

- (NSString *)getValueForIndex:(NSInteger)index
                    withPoints:(NSMutableArray *)points
{
    double x[points.count];
    double y[points.count];
    double min_x = INFINITY;
    double max_x = 0;
    double min_y = INFINITY;
    double max_y = 0;
    for(int i = 0; i < points.count; i++)
    {
        NSValue *v = [points objectAtIndex:i];
        CGPoint p = v.CGPointValue;
        if(p.x < min_x)
            min_x = p.x;
        if(p.x > max_x)
            max_x = p.x;
        if(p.y < min_y)
            min_y = p.y;
        if(p.y > max_y)
            max_y = p.y;
        x[i] = p.x;
        y[i] = p.y;
    }
    if(index == 0)
    {
        return [NSString stringWithFormat:@"%.2f", (max_x - min_x)];
    }
    else if(index == 1)
    {
        return [NSString stringWithFormat:@"%.2f", (max_y - min_y)];
    }
    else if(index == 2)
    {
        return [NSString stringWithFormat:@"%.2f", (MaximumSpread(x, y, points.count))];
    }
    else if(index == 3)
    {
        return [NSString stringWithFormat:@"%.2f", (Mean(x, points.count))];
    }
    else if(index == 4)
    {
        return [NSString stringWithFormat:@"%.2f", (Mean(y, points.count))];
    }
    else if(index == 5)
    {
        return [NSString stringWithFormat:@"%.2f", (StandardDeviation(x, points.count))];
    }
    else if(index == 6)
    {
        return [NSString stringWithFormat:@"%.2f", (StandardDeviation(y, points.count))];
    }
    else if(index == 7)
    {
        return [NSString stringWithFormat:@"%.2f", min_x];
    }
    else if(index == 8)
    {
        return [NSString stringWithFormat:@"%.2f", max_x];
    }
    else if(index == 9)
    {
        return [NSString stringWithFormat:@"%.2f", min_y];
    }
    else if(index == 10)
    {
        return [NSString stringWithFormat:@"%.2f", max_y];
    }
    return @"";
}

@end
