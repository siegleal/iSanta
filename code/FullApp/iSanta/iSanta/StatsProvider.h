//
//  NSObject+StatsProvider.h
//  iSanta
//
//  Created by Eric Henderson on 3/28/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol StatsProvider <NSObject>

- (NSInteger)getRowCount:(NSMutableArray *)points;
- (NSString *)getTitleForIndex:(NSInteger)index withPoints:(NSMutableArray *)points;
- (NSString *)getValueForIndex:(NSInteger)index withPoints:(NSMutableArray *)points;

@end