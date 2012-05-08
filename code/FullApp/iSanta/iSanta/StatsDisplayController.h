//
//  StatsDisplayController.h
//  iSanta
//
//  Created by Eric Henderson on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "StatsProvider.h"
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>

@interface StatsDisplayController : UITableViewController <MFMailComposeViewControllerDelegate>

@property (strong, nonatomic) id<StatsProvider> statsProvider;
@property (strong, nonatomic) NSMutableArray *points;
@property (nonatomic, retain) NSDictionary *reportData;
@property (strong, nonatomic) NSData *targetPhoto;

@end
