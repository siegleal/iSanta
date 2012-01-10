//
//  iSanta_Point.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Photo_Information;

@interface iSanta_Point : NSManagedObject

@property (nonatomic, retain) NSNumber * x;
@property (nonatomic, retain) NSNumber * y;
@property (nonatomic, retain) Photo_Information *photo;

@end
