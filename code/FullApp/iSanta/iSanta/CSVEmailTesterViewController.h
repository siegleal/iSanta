//
//  CSVEmailTesterViewController.h
//  CSVEmailTester
//
//  Created by Eric Henderson on 2/8/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>

@interface CSVEmailTesterViewController : UITableViewController <MFMailComposeViewControllerDelegate>

@property (nonatomic, strong) NSData *targetImage;
@property (nonatomic, strong) NSMutableArray *points;
@property (nonatomic, strong) NSString *mimeType;
@property (nonatomic, strong) NSString *imageFileName;
@property (nonatomic, strong) NSDictionary *stats;
@property (nonatomic, strong) NSDictionary *generalInfo;

@end
