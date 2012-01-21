//
//  PlacementBrain.h
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef struct {
    int x;
    int y;
} ASpoint;

@interface Impact : NSObject
@property ASpoint position;
@end

@interface PlacementBrain : NSObject

@property (nonatomic, strong) UIImage *targetImage;
@property (nonatomic, strong) NSMutableSet *points;


@end
