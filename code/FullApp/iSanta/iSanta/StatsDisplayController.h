//
//  StatsDisplayController.h
//  iSanta
//
//  Created by Eric Henderson on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "StatsProvider.h"

@interface StatsDisplayController : UITableViewController

@property (strong, nonatomic) id<StatsProvider> statsProvider;
@property (strong, nonatomic) NSMutableArray *points;
@property (nonatomic, retain) NSDictionary *reportData;

@end
