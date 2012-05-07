//
//  DefaultStatsProvider.m
//  iSanta
//
//  Created by Eric Henderson on 5/7/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "DefaultStatsProvider.h"

@implementation DefaultStatsProvider

- (NSInteger)getRowCount:(NSMutableArray *)points
{
    return 0;
}

- (NSString *)getTitleForIndex:(NSInteger)index
                    withPoints:(NSMutableArray *)points
{
    return @"";
}

- (NSString *)getValueForIndex:(NSInteger)index
                    withPoints:(NSMutableArray *)points
{
    return @"";
}

@end
