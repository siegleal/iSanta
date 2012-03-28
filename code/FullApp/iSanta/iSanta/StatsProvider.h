//
//  NSObject+StatsProvider.h
//  iSanta
//
//  Created by Eric Henderson on 3/28/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Photo_Information.h"

@protocol StatsProvider <NSObject>

- (NSDictionary *)getStats:(NSMutableArray *)points;

@end